let map;
let circle;

ymaps.ready(init);

function init() {
    const center = getInitialCenter();

    map = createMap(center);
    console.log("map created");

    const locationButton = createLocationButton();
    map.controls.add(locationButton);
    console.log("btn added");

    const radius = getInitialRadius();
    console.log(radius);
    circle = createCircle(center, radius);
    map.geoObjects.add(circle);
    console.log("circle added");

    circle.editor.startEditing();

    // получаем центр карты/начального круга
    // если есть доступ к геолокации, то геолокация
    // иначе центр города
    function getInitialCenter() {
        const lat = getFloat("#lat");
        console.log(lat);
        const lon = getFloat("#lon");
        console.log(lon);
        return [lat, lon];

        function getFloat(tag) {
            const value = $(tag)[0].value;
            const parsableValue = value.replace(",", ".");
            return parseFloat(parsableValue);
        }
    }

    function getInitialRadius() {
        return parseInt($("#radius")[0].value);
    }

    function createMap(center) {
        return new ymaps.Map('map', {
            center: center,
            zoom: 15,
            controls: [
                'rulerControl',
                'searchControl',
                'trafficControl',
                'typeSelector',
                'zoomControl',
                'fullscreenControl',
            ]
        }, {
            searchControlProvider: 'yandex#search'
        });
    }

    function createLocationButton() {
        return new ymaps.control.Button({
            data: {
                content: "Set as Geolocation"
            },
            options: {
                maxWidth: [28, 150, 178]
            }
        });
    }

    function createCircle(center, radius) {
        return new ymaps.Circle([center, radius], {}, {
            fillColor: "#B3D1FC77",
            strokeColor: "#94C3F6",
            strokeOpacity: 0.8,
            strokeWidth: 2
        });
    }
}

function onApplyLocation() {
    const geometry = circle.geometry;
    const coords = geometry.getCoordinates();
    const lat = coords[0];
    const lon = coords[1];
    const radius = geometry.getRadius();
    console.log(lat);
    console.log(lon);
    console.log(radius);
    $("#lat")[0].value = lat;
    $("#lon")[0].value = lon;
    $("#radius")[0].value = radius;
}