import React from "react";
import { makeStyles } from "@material-ui/core/styles";
import Table from "@material-ui/core/Table";
import TableBody from "@material-ui/core/TableBody";
import TableCell from "@material-ui/core/TableCell";
import TableHead from "@material-ui/core/TableHead";
import TableRow from "@material-ui/core/TableRow";
import Paper from "@material-ui/core/Paper";
//Components

const useStyles = makeStyles({
  root: {
    width: "100%",
    overflowX: "auto"
  },
  table: {
    minWidth: 650
  }
});

function createData(
  time,
  healthRisk,
  status,
  region,
  district,
  village,
  dataCollector,
  maleUnderFive,
  femaleUnderFive,
  maleOverFive,
  femaleOverFive
) {
  return {
    time,
    healthRisk,
    status,
    region,
    district,
    village,
    dataCollector,
    maleUnderFive,
    femaleUnderFive,
    maleOverFive,
    femaleOverFive
  };
}

const rows = [
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  ),
  createData(
    "14.10.2019",
    "Measles",
    "Success",
    "Nothern Province",
    "Banbali",
    "Sanda loko",
    "Adama",
    1,
    0,
    0,
    1,
    0,
    1,
    0
  )
];

export default function MainListComponent() {
  const classes = useStyles();

  return (
    <div>
      <Paper className={classes.root}>
        <Table className={classes.table} aria-label="simple table">
          <TableHead>
            <TableRow>
              <TableCell align="right">Time</TableCell>
              <TableCell align="right">Health risk</TableCell>
              <TableCell align="right">Status</TableCell>
              <TableCell align="right">Region</TableCell>
              <TableCell align="right">District</TableCell>
              <TableCell align="right">Village</TableCell>
              <TableCell align="right">Data collector</TableCell>
              <TableCell align="right">Male under 5 </TableCell>
              <TableCell align="right">Female under 5 </TableCell>
              <TableCell align="right">Male over 5 </TableCell>
              <TableCell align="right">Female over 5 </TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {rows.map(row => (
              <TableRow key={row.name}>
                <TableCell component="th" scope="row">
                  {row.time}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.healthRisk}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.status}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.region}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.district}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.village}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.dataCollector}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.maleUnderFive}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.maleOverFive}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.femaleUnderFive}
                </TableCell>
                <TableCell component="th" scope="row">
                  {row.femaleOverFive}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </Paper>
    </div>
  );
}
