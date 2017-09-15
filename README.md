# UniverRepo1
ПМІ-32: Програмування та підтримка веб-застосувань

## Launch app (Windows):
- navigate to `/host/` folder
- create empty folders `temp` and `logs`
- run `start nginx` inside `/host/` folder
- launch http://localhost:12345 in browser

## Troubleshooting
- check if nginx started: run `tasklist /fi "imagename eq nginx.exe"` command
- navigate to `/host/logs` and inspect files `error.log` and `access.log`