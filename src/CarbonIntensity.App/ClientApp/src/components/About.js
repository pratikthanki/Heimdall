import React, { Component } from 'react';

export class About extends Component {
  static displayName = About.name;

  render () {
    return (
      <div>
        <h3>About</h3>
        <p>
          The <a target="_blank" href='https://www.carbonintensity.org.uk/'>Carbon Intensity API</a> uses Machine Learning and power system
          modelling to forecast the carbon intensity and generation mix 96+ hours ahead for each region in the UK.
        </p>

        <p>The National Grid ESO has partnered with:</p>
        <ul>
          <li><a target="_blank" href='https://www.edfeurope.org/'>Environmental Defense Fund Europe</a></li>
          <li><a target="_blank" href='http://www.cs.ox.ac.uk/'>University of Oxford Department of Computer Science</a></li>
          <li><a target="_blank" href='https://www.wwf.org.uk/what-we-do/uk-global-footprint'>World Wildlife Fund (WWF)</a></li>
        </ul>

        <p>
          The forecasts here include CO2 emissions related to electricity generation only. Data here has been collated to 
          provide guidance on optimal periods of the day to comsumer more or less energy and where it comes from. The data 
          provides an indicative trend of regional carbon intensity of the electricity system in the UK.
          <br></br>
          <br></br>
          Forecasted usage also includes emissions from all large metered power stations, interconnector imports, transmission 
          and distribution losses, and accounts for national electricity demand, embedded wind and solar generation:
        </p>
        <ul className="columns" data-columns="2" style={{columns: 2}}>
          <li>Gas</li>
          <li>Coal</li>
          <li>Biomass</li>
          <li>Nuclear</li>
          <li>Hydro</li>
          <li>Storage</li>
          <li>Imports</li>
          <li>Other</li>
          <li>Wind</li>
          <li>Solar</li>
        </ul>

        <h3>What can you see here?</h3>
        <p>
          See <a href='/current-usage'>Current Usage</a> across the UK by region for the last completed 30 minute period.
        </p>
        <p>
          Or, <a href='/forecasts'>Forecasts</a> for the next 96 hours.
        </p>

        <h3>Feedback</h3>
        <p>
          If you have any queries or suggestions on how this can be improved feel free to <a target="_blank" href='https://pratikthanki.github.io?s=carbon-intensity'>get in touch</a>.
        </p>

      </div>
    );
  }
}