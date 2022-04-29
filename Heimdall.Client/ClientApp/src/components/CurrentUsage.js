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

  static renderForecastsTable(forecastsByRegion) {
    return (
      <div>
          <p>
            Carbon intensity (gCO2/kWh) by regions across the UK, 
            between <strong>{forecastsByRegion[0].from.replace(':00+00:00', '').replace('T',' ')}</strong> and <strong>{forecastsByRegion[0].to.replace(':00+00:00', '').replace('T',' ')}</strong>
          </p>
          <table className='table table-striped' aria-labelledby="tabelLabel">
            <thead>
              <tr>
                <th>Shortname</th>
                <th>Carbon Intensity</th>
                <th>Description</th>
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
                <tr key={value.date}>
                  <td>{value.shortname}</td>
                  <td>{value.forecast}</td>
                  <td>{value.forecastDescription}</td>
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
        <h1 id="tabelLabel">Carbon Intensity Forecasts</h1>
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
