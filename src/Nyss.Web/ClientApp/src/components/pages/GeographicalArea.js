import React from "react";
import { connect } from "react-redux";

const GeographicalArea = props => {
  return (
    <div>
      <p>{"Geographical Area"}</p>
    </div>
  );
};

export default connect()(GeographicalArea);
