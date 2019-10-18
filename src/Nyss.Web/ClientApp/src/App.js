import React from "react";
import { Route } from "react-router";
import Layout from "./components/Layout";
import Home from "./components/Home";
import Dashboard from "./components/pages/Dashboard";
import Projects from "./components/pages/Projects";
import Users from "./components/pages/Users";
import ReportList from "./components/pages/ReportList";
import GeographicalArea from "./components/pages/GeographicalArea";
import SMSGateways from "./components/pages/SMSGateways";

export default () => (
  <Layout>
    <Route exact path="/" component={Home} />
    <Route exact path="/dashboard" component={Dashboard} />
    <Route exact path="/projects" component={Projects} />
    <Route exact path="/users" component={Users} />
    <Route exact path="/report-list" component={ReportList} />
    <Route exact path="/geographical-area" component={GeographicalArea} />
    <Route exact path="/sms-gateways" component={SMSGateways} />
  </Layout>
);
