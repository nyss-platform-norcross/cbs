import React, { useEffect, useRef } from 'react';
import { Map, TileLayer, Marker, Popup } from 'react-leaflet';

import './MapReports.css';

const MapReports = () => {
 
  const position = [51.505, -0.09];

  return (
    <Map center={position} zoom={13}>
      <TileLayer
        url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
        attribution="&copy; <a href=&quot;http://osm.org/copyright&quot;>OpenStreetMap</a> contributors"
      />
      <Marker position={position}>
        <Popup>
          A pretty CSS3 popup. <br/> Easily customizable.
        </Popup>
      </Marker>
    </Map>
  )
}

export default MapReports;