import React, { Component } from "react";

export class CurrentUsage extends Component {
  static displayName = CurrentUsage.name;

  constructor(props) {
    super(props);
    this.state = { 
      forecastsByRegion: [], 
      loading: true
    };
  }

  componentDidMount() {
    this.populateCarbonData();
  }

  static rowColor = (d) => {
    var style = '';
    if (d === 'very high') {
      style='veryhigh';
    } else if (d === 'high') {
      style='high';
    } else if (d === 'moderate') {
      style='high';
    } else if (d === 'low') {
      style='low';
    } else if (d === 'very low') {
      style='verylow';
    } else {
      style='';
    }

    return style;
 }

 static formatDatetime = (date) => {
  return date.replace(':00+00:00', '').replace('T',' ').split('+')[0]
 }

  static renderForecastsTable(forecastsByRegion) {
   return (
      <div>
          <p>
            Carbon intensity (gCO2/kWh) by regions across the UK, 
            between <strong>{this.formatDatetime(forecastsByRegion[0].from)}</strong> and <strong>{this.formatDatetime(forecastsByRegion[0].to)}</strong> (times in UTC)
          </p>
          <p>
            <b>Key: </b>
            <br></br><b className="veryhigh">Very High</b>
            <br></br><b className="high">High</b>
            <br></br><b className="moderate">Moderate</b>
            <br></br><b className="low">Low</b>
            <br></br><b className="verylow">Very Low</b>
          </p>
          <table className="table table-sm" aria-labelledby="tabelLabel">
            <thead className="thead-light">
              <tr>
                <th>Region</th>
                <th>Carbon Intensity</th>
                {/* <th>Description</th> */}
                <th>Gas</th>
                <th>Coal</th>
                <th>Biomass</th>
                <th>Nuclear</th>
                <th>Hydro</th>
                <th>Storage</th>
                <th>Imports</th>
                <th>Other</th>
                <th>Wind</th>
                <th>Solar</th>
              </tr>
            </thead>
            <tbody>
            {forecastsByRegion.map(value => 
                <tr key={value.shortname}>
                  <td>{value.shortname}</td>
                  <td className={this.rowColor(value.forecastDescription)}>{value.forecast}</td>
                  {/* <td>{value.forecastDescription}</td> */}
                  <td>{value.gas}</td>
                  <td>{value.coal}</td>
                  <td>{value.biomass}</td>
                  <td>{value.nuclear}</td>
                  <td>{value.hydro}</td>
                  <td>{value.storage}</td>
                  <td>{value.imports}</td>
                  <td>{value.other}</td>
                  <td>{value.wind}</td>
                  <td>{value.solar}</td>
                </tr>)}
            </tbody>
          </table>
      </div>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      CurrentUsage.renderForecastsTable(this.state.forecastsByRegion)
    );

    return (
      <div>
        <h1 id="tabelLabel">Current Usage</h1>
        {contents}
      </div>
    );
  }

  async populateCarbonData() {
    const response = await fetch("api/current");
    const data = await response.json();
    this.setState({ forecastsByRegion: data, loading: false });
  }
}
