import React from "react";
import { connect } from "react-redux";
import SimpleTable from "../components/responsive/createData";
const Home = props => (
  <div>
    <SimpleTable />
  </div>
);

export default connect()(Home);
