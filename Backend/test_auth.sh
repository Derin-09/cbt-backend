#!/bin/bash
set -e

# Identify the project file
PROJ_FILE=$(ls *.csproj | head -n 1)
echo "Building project $PROJ_FILE..."

# 1) Build project
dotnet build "$PROJ_FILE" > build_log.txt 2>&1 || (cat build_log.txt && exit 1)

# 2) Start server in background
echo "Starting server on http://localhost:3002..."
dotnet run --project "$PROJ_FILE" --no-build --urls http://localhost:3002 > server_log.txt 2>&1 &
SERVER_PID=$!

# Wait for server to be ready
COUNT=0
while ! curl -s http://localhost:3002 > /dev/null; do
    sleep 2
    COUNT=$((COUNT+1))
    if [ $COUNT -gt 30 ]; then
        echo "Server failed to start"
        kill $SERVER_PID
        exit 1
    fi
done
echo "Server is up."

# 3) Generate unique values
EMAIL="admin_$(date +%s)@example.com"
MATRIC="MATRIC_$(date +%s)"

# 4) Register Admin
echo "Registering Admin..."
REG_ADMIN_RES=$(curl -s -w "\n%{http_code}" -X POST http://localhost:3002/api/Auth/register/admin \
    -H "Content-Type: application/json" \
    -d "{\"fullName\":\"Admin User\",\"password\":\"Admin12345\",\"position\":\"Principal\",\"email\":\"$EMAIL\"}")
REG_ADMIN_STATUS=$(echo "$REG_ADMIN_RES" | tail -n1)
REG_ADMIN_BODY=$(echo "$REG_ADMIN_RES" | sed '$d')

# 5) Login Admin
echo "Logging in Admin..."
LOGIN_RES=$(curl -s -i -w "\n%{http_code}" -X POST http://localhost:3002/api/Auth/login/admin \
    -c cookies.txt \
    -H "Content-Type: application/json" \
    -d "{\"email\":\"$EMAIL\",\"password\":\"Admin12345\"}")
LOGIN_STATUS=$(echo "$LOGIN_RES" | tail -n1)
LOGIN_HEADERS=$(echo "$LOGIN_RES" | grep -i "Set-Cookie")
LOGIN_BODY=$(echo "$LOGIN_RES" | sed '$d' | sed '1,/^\r$/d')

# 6) Register Student (using cookies)
echo "Registering Student..."
REG_STUDENT_RES=$(curl -s -w "\n%{http_code}" -X POST http://localhost:3002/api/Auth/register/student \
    -b cookies.txt \
    -H "Content-Type: application/json" \
    -d "{\"fullName\":\"Student User\",\"password\":\"Stud12345\",\"department\":\"Science\",\"matricNo\":\"$MATRIC\"}")
REG_STUDENT_STATUS=$(echo "$REG_STUDENT_RES" | tail -n1)
REG_STUDENT_BODY=$(echo "$REG_STUDENT_RES" | sed '$d')

# 7) Print results
echo "----------------------------------------"
echo "Admin Registration: Status $REG_ADMIN_STATUS, Body: $REG_ADMIN_BODY"
echo "Admin Login: Status $LOGIN_STATUS"
echo "Login Cookies: $LOGIN_HEADERS"
echo "Student Registration: Status $REG_STUDENT_STATUS, Body: $REG_STUDENT_BODY"
echo "----------------------------------------"

# 8) Cleanup
kill $SERVER_PID
rm cookies.txt test_auth.sh build_log.txt server_log.txt
