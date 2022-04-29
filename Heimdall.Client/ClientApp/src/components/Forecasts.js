import React, { Component } from "react";
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

export class Forecasts extends Component {
  static displayName = Forecasts.name;

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
        <div>
        {Object.entries(forecastsByRegion).map(([key, value]) => 
          <div>
              <h3 key="{key}">{key}</h3>
              <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                  <tr>
                    <th>From</th>
                    <th>To</th>
                    {/* <th>Shortname</th> */}
                    <th>Forecast</th>
                    <th>Forecast Description</th>
                    {/* <th>Gas</th>
                    <th>Coal</th>
                    <th>Biomass</th>
                    <th>Nuclear</th>
                    <th>Hydro</th>
                    <th>Storage</th>
                    <th>Imports</th>
                    <th>Other</th>
                    <th>Wind</th>
                    <th>Solar</th>
                    <th>IsForecast</th> */}
                  </tr>
                </thead>
                <tbody>
                  {value.map(forecast =>
                    <tr key={forecast.date}>
                      <td>{forecast.from.replace(':00+00:00', '').replace('T',' ')}</td>
                      <td>{forecast.to.replace(':00+00:00', '').replace('T',' ')}</td>
                      {/* <td>{forecast.shortname}</td> */}
                      <td>{forecast.forecast}</td>
                      <td>{forecast.forecastDescription}</td>
                      {/* <td>{forecast.gas}</td>
                      <td>{forecast.coal}</td>
                      <td>{forecast.biomass}</td>
                      <td>{forecast.nuclear}</td>
                      <td>{forecast.hydro}</td>
                      <td>{forecast.storage}</td>
                      <td>{forecast.imports}</td>
                      <td>{forecast.other}</td>
                      <td>{forecast.wind}</td>
                      <td>{forecast.solar}</td>
                      <td>{forecast.isForecast}</td> */}
                    </tr>
                  )}
                </tbody>
              </table>
              </div>)}
        </div>

        {/* <div>
          {Object.entries(forecastsByRegion).map(([key, value]) => (
            <div>
              <h4 key="{key}">{key}</h4>

              <Line options={options} data={data} />

              {value.map(forecast => 
                [{ x: forecast.from, y: forecast.gas }]
                )}
            </div>
          ))}
        </div> */}

      </div>
    );
  }

  render() {
    let contents = this.state.loading ? (
      <p>
        <em>Loading...</em>
      </p>
    ) : (
      Forecasts.renderForecastsTable(this.state.forecastsByRegion)
    );

    return (
      <div>
        <h1 id="tabelLabel">Carbon Intensity Forecasts</h1>
        <p>Carbon forecasts for the next 96 hours by regions across the UK.</p>
        {contents}
      </div>
    );
  }

  async populateCarbonData() {
    const response = await fetch("api/forecast");
    const data = await response.json();
    this.setState({ forecastsByRegion: data, loading: false });
  }
}
