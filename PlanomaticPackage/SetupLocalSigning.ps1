echo "This helper utility will create a local certificate that can be used for local test builds of this project."
echo "Azure pipeline builds will use a different cert stored in the secure vault."
echo "The official release will be signed by the microsoft internal signing service."

# This certificate subject name must match what is in the package manifest
$CertSubject = "CN=Planomatic, O=Planomatic Project, C=US"
$CertFile = "certificate.pfx"

echo "Checking for existing certificate"
if (Test-Path -path $CertFile)
{
    echo "Certificate is already setup here, exiting"
    return
}

echo "Creating new self signed certificate"
New-SelfSignedCertificate -Type Custom -Subject "CN=Planomatic, O=Planomatic Project, C=US" -KeyUsage DigitalSignature -FriendlyName "Planomatic local certificate" -CertStoreLocation "Cert:\CurrentUser\My" -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")

echo "Exporting new self signed certificate"
# Find the certificate we just grabbed
$NewCert = Get-ChildItem cert:\CurrentUser\My | Where-Object {$_.Subject -eq $CertSubject}
$password = ConvertTo-SecureString -String "1234" -Force -AsPlainText
$CertThumbprint = $NewCert.Thumbprint
Export-PfxCertificate -cert "Cert:\CurrentUser\My\$CertThumbprint" -FilePath $CertFile -Password $password

echo "Deleting certificate from local store"
Remove-Item "Cert:\CurrentUser\My\$CertThumbprint"

