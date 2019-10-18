import React from "react";
import NavMenu from "./NavMenu";
import SideBar from "./SideBar";
import "../styles/screen.css";

export default props => (
  <div>
    <NavMenu />
    <div className={"custom-container"} style={{ paddingLeft: "272px" }}>
      <SideBar />
      {props.children}
    </div>
  </div>
);
