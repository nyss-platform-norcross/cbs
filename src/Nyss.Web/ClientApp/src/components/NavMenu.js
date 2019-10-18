import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Popover from "@material-ui/core/Popover";
import Typography from "@material-ui/core/Typography";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";
import "./NavMenu.css";
import PersonIcon from "@material-ui/icons/Person";
import ArrowDropDownIcon from "@material-ui/icons/ArrowDropDown";
import N_RedCrossLogo from "../assets/images/n-red-cross-logo.png";

const useStyles = makeStyles(theme => ({
  typography: {
    padding: theme.spacing(2),
    fontSize: "12px"
  }
}));

const NavMenu = () => {
  const classes = useStyles();
  const [anchorEl, setAnchorEl] = React.useState(null);

  const handleClick = event => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const open = Boolean(anchorEl);
  const id = open ? "simple-popover" : undefined;

  return (
    <header>
      <div className="navbar-container box-shadow">
        <div className={"custom-container"}>
          <div className={"header-content custom-row"}>
            <div className={"float-left logo-container"}>
              <img
                id={"nRedCrossLogo"}
                src={N_RedCrossLogo}
                alt="Red Cross logo"
              />
            </div>

            <div className={"float-right user-menu"}>
              <ClickAwayListener onClickAway={handleClose}>
                <div
                  aria-describedby={id}
                  className={"user"}
                  onClick={handleClick}
                >
                  <PersonIcon className={"user-icon"} />
                  <span className={"user-name"}>{"Krzysztof Jeske"}</span>
                  <ArrowDropDownIcon className={"arrow-icon"} />
                  <span className={"user-role"}>{"Data owner"}</span>
                  <Popover
                    id={id}
                    open={open}
                    anchorEl={anchorEl}
                    anchorOrigin={{
                      vertical: "bottom",
                      horizontal: "center"
                    }}
                    transformOrigin={{
                      vertical: "top",
                      horizontal: "center"
                    }}
                  >
                    <Typography className={classes.typography}>
                      {"User menu content here"}
                    </Typography>
                  </Popover>
                </div>
              </ClickAwayListener>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};
export default NavMenu;
