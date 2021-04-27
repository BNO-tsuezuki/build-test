import * as React from 'react';
import { DragDropContext } from 'react-dnd';
import HTML5Backend from 'react-dnd-html5-backend';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';

import { store } from 'store/_App';

import Header from 'components/Header';
import Contents from 'components/Contents';
import Footer from 'components/Footer';
import ModalOverlay from 'components/ModalOverlay';

class App extends React.Component<{}, {}>{

  render() {
    return <>
      <div style={{ position:"fixed", left:"0%", top:"0%", right:"0%", height:"40px", background:"rgba(255,0,0,0.2)"}}>
        <Header />
      </div>
      
      <div style={{ position:"fixed", left:"0%", top:"40px", right:"0%", bottom:"20px"}}>
        <Contents />
      </div>
      
      <div style={{ position:"fixed", left:"0%", right:"0%", bottom:"0%", height:"20px", background:"rgba(0,0,255,0.2)"}}>
        <Footer />
      </div>

      
      <ModalOverlay />
    </>;
  }
}

const AppDnd = DragDropContext(HTML5Backend)(App);

ReactDOM.render(
  <Provider store={store}>
    <AppDnd />
  </Provider>,
  document.getElementById('app')
);
