# CryptoSniper

# Summary
CryptoSniper is an automated cryptocurrency trading application. 

## Tools required 
- Visual Studio with .NET Framework 4.6.1 or greater (to build service executable)
- MySQL Database

## Deployment
1. Inside a 'run as admin' bash / command line tool run the following command:
> CryptoSniper install start -instance: -config:c:\config.json
- config.json is based on `config.json.template`
To uninstall/stop run the following command:
> CryptoSniper uninstall

# Development

## Database
A database must be created with the following criteria inside:
`crypto_sniper_*.sql`