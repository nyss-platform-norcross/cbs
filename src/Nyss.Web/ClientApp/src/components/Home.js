import React from "react";
import { connect } from "react-redux";
import Typography from "@material-ui/core/Typography";

const Home = props => {
  return (
    <main>
      <Typography paragraph>{"Main"}</Typography>
    </main>
  );
};

export default connect()(Home);
