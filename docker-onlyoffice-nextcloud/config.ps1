# Enable verbose output
$VerbosePreference = "Continue"

# Execute command to get trusted domains and append to file
docker exec -u www-data app-server php occ --no-warnings config:system:get trusted_domains | Out-File -Append -FilePath trusted_domain.tmp

# Check if "nginx-server" is in the file, if not, add it
if (!(Get-Content trusted_domain.tmp | Select-String -Pattern "nginx-server")) {
    $TRUSTED_INDEX = Get-Content trusted_domain.tmp | Measure-Object | Select-Object -ExpandProperty Count
    docker exec -u www-data app-server php occ --no-warnings config:system:set trusted_domains $TRUSTED_INDEX --value="nginx-server"
}

# Remove temporary file
Remove-Item trusted_domain.tmp

# Install OnlyOffice app
docker exec -u www-data app-server php occ --no-warnings app:install onlyoffice

# Configure OnlyOffice settings
docker exec -u www-data app-server php occ --no-warnings config:system:set onlyoffice DocumentServerUrl --value="/ds-vpath/"
docker exec -u www-data app-server php occ --no-warnings config:system:set onlyoffice DocumentServerInternalUrl --value="http://onlyoffice-document-server/"
docker exec -u www-data app-server php occ --no-warnings config:system:set onlyoffice StorageUrl --value="http://nginx-server/"
docker exec -u www-data app-server php occ --no-warnings config:system:set onlyoffice jwt_secret --value="secret"