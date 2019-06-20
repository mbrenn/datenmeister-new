$moduleName = "DatenMeister.Icons"

cd ..
cd modules


if (Test-Path $moduleName) {
	Write-Host "Pulling DatenMeister.Icon"
	cd DatenMeister.Icons
	git pull
	cd ..
}
else {
	Write-Host "Cloning DatenMeister.Icons"
	mkdir DatenMeister.Icons
	cd DatenMeister.Icons
	git clone -b master ssh://mbrenn@ratte-ubuntu/~/git/DatenMeister.Icons .
	cd ..
}


cd ..
cd scripts