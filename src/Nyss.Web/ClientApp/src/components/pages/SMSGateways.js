import React from "react";
import { connect } from "react-redux";

const SMSGateways = props => {
  return (
    <div>
      <p>{"SMS Gateways"}</p>
    </div>
  );
};

export default connect()(SMSGateways);
