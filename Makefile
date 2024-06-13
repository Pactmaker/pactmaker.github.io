build:
	dotnet build
clean:
	dotnet clean
	rm -rf */bin/
	rm -rf */obj/
watch:
	dotnet watch run --project Pactmaker.Wasm --verbose
watch_prod:
	dotnet watch run --project Pactmaker.Wasm --verbose --launch-profile Packmaker.Wasm-Production
run:
	dotnet run --project Pactmaker.Wasm
run_prod:
	dotnet run --project Pactmaker.Wasm --launch-profile Packmaker.Wasm-Production
publish:
	dotnet publish

../wixoss-data/assets/zh-Hans/cards.json:
	cd ../wixoss-data && git reset --hard FETCH_HEAD && git clean -ffd
	../wixoss-data/scripts/wixoss.cli data scrub ../wixoss-data/assets/zh-Hans/
	../wixoss-data/scripts/create_cardlist

update_assets: ../wixoss-data/assets/zh-Hans/cards.json
	rm -rf Pactmaker.Wasm/wwwroot/assets
	ln -sf ../../../wixoss-data/assets/ Pactmaker.Wasm/wwwroot/assets

update_assets_prod: ../wixoss-data/assets/zh-Hans/cards.json
	rm -rf Pactmaker.Wasm/wwwroot/assets
	mkdir -p Pactmaker.Wasm/wwwroot/assets/zh-Hans/
	cp ../wixoss-data/assets/zh-Hans/cards.json Pactmaker.Wasm/wwwroot/assets/zh-Hans/
