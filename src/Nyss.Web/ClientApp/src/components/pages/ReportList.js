import React from "react";
import { connect } from "react-redux";
import ReportListComponent from "./report-list-components/ReportListComponent";

const ReportList = props => {
  return <ReportListComponent />;
};

export default connect()(ReportList);
