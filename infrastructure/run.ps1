Set-Item -Path env:TF_VAR_s3_object_version -Value $(aws s3api list-object-versions --bucket moneymatters --prefix lambda-packages/tesco_statement_handler/package.zip --query Versions[?IsLatest].VersionId --output text)

Write-Host $env:TF_VAR_s3_object_version

terraform apply