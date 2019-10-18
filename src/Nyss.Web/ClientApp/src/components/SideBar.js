import React from "react";
import { connect } from "react-redux";
import { NavLink } from "react-router-dom";
import List from "@material-ui/core/List";
import ListItem from "@material-ui/core/ListItem";
import ListItemText from "@material-ui/core/ListItemText";
import { withRouter } from "react-router";

const SideBar = props => {
  return (
    <div className={"sidebar-menu-container"}>
      <List className={"sidebar-menu"}>
        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/dashboard"}
          exact={true}
        >
          <ListItem className={"list-item"} button>
            <ListItemText className={"list-item-text"} primary="Dashboard" />
          </ListItem>
        </NavLink>

        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/projects"}
          exact={true}
        >
          <ListItem className={"list-item"} button>
            <ListItemText className={"list-item-text"} primary="Projects" />
          </ListItem>
        </NavLink>

        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/users"}
        >
          <ListItem className={"list-item"} button>
            <ListItemText className={"list-item-text"} primary="Users" />
          </ListItem>
        </NavLink>

        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/report-list"}
          exact={true}
        >
          <ListItem className={"list-item"} button>
            <ListItemText className={"list-item-text"} primary="Report List" />
          </ListItem>
        </NavLink>

        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/geographical-area"}
          exact={true}
        >
          <ListItem className={"list-item"} button>
            <ListItemText
              className={"list-item-text"}
              primary="Geographical area"
            />
          </ListItem>
        </NavLink>

        <NavLink
          className={"list-item-container"}
          activeClassName="active"
          to={"/sms-gateways"}
          exact={true}
        >
          <ListItem className={"list-item"} button>
            <ListItemText className={"list-item-text"} primary="SMS gateways" />
          </ListItem>
        </NavLink>
      </List>
    </div>
  );
};

export default withRouter(connect()(SideBar));
