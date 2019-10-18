import React from "react";
import { connect } from "react-redux";

const Users = props => {
  return (
    <div>
      <p>{"User management component"}</p>
    </div>
  );
};

export default connect()(Users);
