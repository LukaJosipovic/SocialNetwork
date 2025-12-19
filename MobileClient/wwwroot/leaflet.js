let map;
let marker;
export function load_map(latitude, longitude) {
	const mapDiv = document.getElementById('map');

	// if map exists but container was destroyed, reset it
	if (map && !mapDiv.hasChildNodes()) {
		map.remove();  // clean up old map
		map = null;
		marker = null;
	}

	if (!map) {
		map = L.map('map').setView([latitude, longitude], 13);
		marker = L.marker([latitude, longitude]).addTo(map);
		var Stadia_AlidadeSmoothDark = L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', {
			minZoom: 13,
			maxZoom: 20,
			attribution: '&copy; <a href="https://www.stadiamaps.com/" target="_blank">Stadia Maps</a> &copy; <a href="https://openmaptiles.org/" target="_blank">OpenMapTiles</a> &copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
			ext: 'png'
		}).addTo(map);
	}
	else {
		marker.setLatLng([latitude, longitude]);
		map.setView([latitude, longitude], 13);
	}
}
export function add_user_markers(usersJson) {
	const users = JSON.parse(usersJson);
	const userMarkers = {};
	// Clear existing markers if needed
	for (const id in userMarkers) {
		map.removeLayer(userMarkers[id]);
	}

	for (const user of users) {
		debugger
		if (user.Latitude && user.Longitude) {
			let imageHtml = "";
			if (user.ProfilePicture && user.ProfilePicture.length > 0) {
				//imageHtml = `<img src="${user.ProfilePictureString}" alt="User picture" style="width:100px;height:auto;display:block;margin-bottom:5px;" />`; 
				imageHtml = `<img src="${user.ProfilePictureString}" alt="User picture" style="width:100px;height:100px;object-fit:cover;border-radius:50%;display:block;margin:0 auto 10px auto;" />`; 
			}

			const popupContent = `
                <div style="text-align:center;">
					${imageHtml}
					<b>${user.Name}</b>
				</div>
            `;

			const marker = L.marker([user.Latitude, user.Longitude])
				.addTo(map)
				.bindPopup(popupContent);
			userMarkers[user.Id] = marker;
		}
	}
}

//export function addOrMoveUserMarker(userId, lat, lng) {
//	if (!map) return;

//	if (userMarkers[userId]) {
//		// Move existing marker
//		userMarkers[userId].setLatLng([lat, lng]);
//	} else {
//		// Add new marker and remember it
//		const m = L.marker([lat, lng]).addTo(map);
//		userMarkers[userId] = m;
//	}
//}

//let map;
//let markers = {}; // multiple markers

//export function load_map(latitude, longitude) {
//    if (!map) {
//        map = L.map('map').setView([latitude, longitude], 13);
//        L.tileLayer('https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png', {
//            minZoom: 13,
//            maxZoom: 20,
//        }).addTo(map);
//    }
//    upsert_marker('me', latitude, longitude);
//}

//export function upsert_marker(userId, lat, lng) {
//    if (!map) return;

//    if (markers[userId]) {
//        markers[userId].setLatLng([lat, lng]);
//    } else {
//        const iconColor = userId === 'me' ? 'gold' : 'red';

//        markers[userId] = L.marker([lat, lng], {
//            icon: L.icon({
//                iconUrl: `/pins/${iconColor}.png`,
//                iconSize: [25, 41]
//            })
//        }).addTo(map).bindPopup(userId === 'me' ? 'You' : userId);
//    }
//}