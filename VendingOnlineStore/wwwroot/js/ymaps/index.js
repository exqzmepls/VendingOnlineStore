let circleGeometry;

// noinspection JSUnresolvedVariable
ymaps.ready(init);

async function init() {
    const locationButton = createLocationButton();

    const center = await getCenter();
    console.log(center);

    const radius = getRadius();

    const circle = createCircle(center, radius);
    // noinspection JSUnresolvedVariable
    circleGeometry = circle.geometry;

    const objectManager = createObjectManager();
    tryLoadObjectManagerData(objectManager);

    const map = createMap(center);
    map.controls.add(locationButton);

    // noinspection JSUnresolvedVariable
    const mapGeoObjects = map.geoObjects;
    mapGeoObjects.add(circle);
    mapGeoObjects.add(objectManager);

    // noinspection JSUnresolvedVariable, JSUnresolvedFunction
    circle.editor.startEditing();

    function createLocationButton() {
        // noinspection JSUnresolvedVariable
        const button = new ymaps.control.Button({
            data: {
                image: "img/geolocation-16.png"
            },
            options: {
                selectOnClick: false,
                maxWidth: [28, 150, 178]
            }
        });
        button.events.add("click", async function () {
            let center = await getCurrentPosition();
            if (center) {
                // noinspection JSUnresolvedFunction
                circleGeometry.setCoordinates(center);

                // noinspection JSUnresolvedFunction
                map.setCenter(center);
            }
        });

        return button;
    }

    async function getCenter() {
        let center = await getCurrentPosition();
        if (!center) {
            $.ajax({
                url: "/Map/DefaultLocation"
            }).done(function (data) {
                console.log(data);
                center = data;
            });
            await sleep(1000);
        }

        return center;
    }

    function getRadius() {
        return 250;
    }

    function createCircle(center, radius) {
        // noinspection JSUnresolvedVariable, JSUnresolvedFunction
        return new ymaps.Circle([center, radius], {}, {
            fillColor: "#B3D1FC77",
            strokeColor: "#94C3F6",
            strokeOpacity: 0.8,
            strokeWidth: 2
        });
    }

    function createObjectManager() {
        // noinspection JSUnresolvedVariable, JSUnresolvedFunction
        const objectManager = new ymaps.ObjectManager({
            clusterize: true,
            gridSize: 32,
            clusterDisableClickZoom: true
        });

        // noinspection JSUnresolvedVariable
        objectManager.objects.options.set('preset', 'islands#greenCircleDotIcon');

        // noinspection JSUnresolvedVariable
        objectManager.clusters.options.set('preset', 'islands#greenClusterIcons');

        return objectManager;
    }

    function tryLoadObjectManagerData(objectManager) {
        $.ajax({
            url: "/Map/PickupPoints"
        }).done(function (data) {
            objectManager.add(data);
        });
    }

    function createMap(center) {
        // noinspection JSUnresolvedVariable, JSUnresolvedFunction
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

    async function getCurrentPosition() {
        let center = null;

        const geolocation = navigator.geolocation;
        if (geolocation) {
            const timeout = 1000;
            const options = {
                enableHighAccuracy: true,
                maximumAge: 10000,
                timeout: timeout
            }
            geolocation.getCurrentPosition(success, handleError, options);
            await sleep(timeout);
        }

        return center;

        function success(position) {
            const coords = position.coords;
            center = [coords.latitude, coords.longitude];
        }

        function handleError(error) {
            alert(error.message);
        }
    }

    function sleep(ms) {
        return new Promise(resolve => setTimeout(resolve, ms));
    }
}

function onApplyLocation() {
    // noinspection JSUnresolvedFunction
    const coords = circleGeometry.getCoordinates();

    // noinspection JSUnresolvedFunction
    const radius = circleGeometry.getRadius();

    setValue("#lat", coords[0]);
    setValue("#lon", coords[1]);
    setValue("#radius", radius);

    function setValue(tag, value) {
        $(tag)[0].value = value;
    }
}