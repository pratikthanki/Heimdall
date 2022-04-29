import React, { Component } from 'react';

export class About extends Component {
  static displayName = About.name;

  render () {
    return (
      <div>
        <h1>About</h1>
        <p>
          The <a href='https://www.carbonintensity.org.uk/'>Carbon Intensity API</a> uses Machine Learning and power system
          modelling to forecast the carbon intensity and generation mix 96+ hours ahead for each region in the UK.
        </p>

        <p>The National Grid ESO has partnered with:</p>
        <ul>
          <li><a href='https://www.edfeurope.org/'>Environmental Defense Fund Europe</a></li>
          <li><a href='http://www.cs.ox.ac.uk/'>University of Oxford Department of Computer Science</a></li>
          <li><a href='https://www.wwf.org.uk/what-we-do/uk-global-footprint'>World Wildlife Fund (WWF)</a></li>
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
      </div>
    );
  }
}