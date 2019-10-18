import React from "react";
import { connect } from "react-redux";

const Dashboard = props => {
  return (
    <div>
      <p>{"Dashboard"}</p>
    </div>
  );
};

export default connect()(Dashboard);
