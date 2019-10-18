import React from "react";
import { connect } from "react-redux";

const ReportList = props => {
  return (
    <div>
      <p>{"Report List"}</p>
    </div>
  );
};

export default connect()(ReportList);
