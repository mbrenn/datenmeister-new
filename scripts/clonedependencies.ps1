$moduleName = "DatenMeister.Icons"

cd ..
cd modules


if (Test-Path $moduleName) {
	Write-Host "Pulling DatenMeister.Icon"
	cd DatenMeiser.Icons
	git pull
	cd ..
	Break Script
}
else {
	Write-Host "Cloning DatenMeister.Icons"
	mkdir DatenMeister.Icons
	git clone -b develop https://github.com/mbrenn/burnsystems.git modules/DatenMeister.Icons
	cd ..
}


cd ..
cd scripts