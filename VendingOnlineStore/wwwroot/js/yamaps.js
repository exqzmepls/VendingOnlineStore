var map;
var circle;

ymaps.ready(init);

function init() {
    const center = getCenter();

    map = createMap(center);

    const locationButton = createLocationButton();
    map.controls.add(locationButton);

    const radius = getRadius();
    circle = createCircle(center, radius);

    map.geoObjects.add(circle);

    circle.editor.startEditing();

    // получаем центр карты/начального круга
    // если есть доступ к геолокации, то геолокация
    // иначе центр города
    function getCenter() {
        return [55.76, 37.64];
    }

    function getRadius() {
        return 1000;
    }

    function createMap(center) {
        return new ymaps.Map('map', {
            center: center,
            zoom: 9,
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
                content: "Location"
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

function applyLocation() {
    const coords = circle.coordinates;
    const lat = coords[0];
    const lon = coords[1];
    const radius = circle.radius;
    $("#lat").value = lat;
    $("#lon").value = lon;
    $("#radius").value = radius;
}