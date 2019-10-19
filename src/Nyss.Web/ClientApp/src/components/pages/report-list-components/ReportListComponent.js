import React from "react";
import MainListComponent from "./MainList";
import TabCreateDataComponent from "./TabCreateDataComponent";
export default function ReportListComponent() {
  return (
    <div>
      <h1>Report list</h1>

      <TabCreateDataComponent />
      <div className={"table-main-list-components"}>
        <MainListComponent />
      </div>
    </div>
  );
}
