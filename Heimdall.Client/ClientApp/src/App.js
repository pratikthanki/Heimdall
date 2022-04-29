import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { About } from './components/About';
import { Forecasts } from './components/Forecasts';
import { CurrentUsage } from './components/CurrentUsage';

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Forecasts} />
        <Route exact path='/current' component={CurrentUsage} />
        <Route exact path='/about' component={About} />
      </Layout>
    );
  }
}
