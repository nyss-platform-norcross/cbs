import React from "react";
import { connect } from "react-redux";
import SimpleTable from "./report-list-components/createData";

const ReportList = props => {
  return <SimpleTable />;
};

export default connect()(ReportList);
