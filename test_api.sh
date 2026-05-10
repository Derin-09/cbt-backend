#!/bin/bash
export ASPNETCORE_URLS="http://localhost:5055"
dotnet run --project Backend/Backend.csproj > api.log 2>&1 &
API_PID=$!

# Wait for API to be ready - check 5055 specifically
timeout=60
while ! curl -s http://localhost:5055 > /dev/null; do
  sleep 2
  ((timeout-=2))
  if [ $timeout -le 0 ]; then
    echo "API failed to start on http://localhost:5055"
    # Fallback: check if it started on 3000 despite environment variable
    if curl -s http://localhost:3000 > /dev/null; then
        echo "API started on port 3000 instead of 5055"
        URL="http://localhost:3000"
        break
    else
        kill $API_PID
        exit 1
    fi
  fi
  URL="http://localhost:5055"
done

echo "Using URL: $URL"

# Send register-admin request
RESPONSE=$(curl -s -i -X POST "$URL/api/auth/register-admin" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@example.com", "password":"Password123!", "username":"admin"}')

echo "--- Response ---"
echo "$RESPONSE"
echo "----------------"

# Kill the API
kill $API_PID
wait $API_PID 2>/dev/null

# Check logs for unhandled exceptions
echo "--- Log Check ---"
if grep -qi "Unhandled Exception" api.log || grep -qi "fail:" api.log || grep -qi "crit:" api.log; then
  echo "Unhandled exceptions or critical errors found in logs."
  grep -Ei "Unhandled Exception|fail:|crit:" api.log | grep -v "Microsoft.EntityFrameworkCore.Database.Command" | head -n 20
else
  echo "No unhandled exceptions found in logs."
fi
