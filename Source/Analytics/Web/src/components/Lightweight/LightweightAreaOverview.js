import React, { Component } from 'react';
import Grid from '@material-ui/core/Grid';
import { Link } from 'react-router-dom';
import HealthRiskSelector from '../healthRisk/HealthRiskSelector';
import CBSNavigation from '../Navigation/CBSNavigation';
import '../../assets/main.scss';

export default class LightweightAreaOverview extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <Grid container>
                <Grid item xs={12}>
                    <CBSNavigation activeMenuItem="analytics/#" />
                </Grid>
                
                <Grid container item xs={12} justify="center">
                    <div className="lightweight">
                        <h1>Light Area Overview</h1>
                        <p>This is the light version of the country overview page. If you want the normal version click <Link to="/analytics">here</Link></p>
                    </div>
                    <HealthRiskSelector />
                </Grid>
            </Grid>
        );
    }
}