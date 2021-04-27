import * as React from 'react';
import { BrowserRouter, Route, NavLink } from 'react-router-dom';


import Startup from 'components/Contents.Startup';
import Achievement from 'components/Contents.Achievement';
import Matchmake from 'components/Contents.Matchmake';
import Account from 'components/Contents.Account';
import GrantItem from 'components/Contents.GrantItem';
import ClientLauncher from 'components/Contents.ClientLauncher';


export default class Contents extends React.Component<{}, {}> {

  render() {

    const linkInfos = [
      { addr: '/',                component: Startup,         caption: 'スタートアップ' },
      { addr: '/achievement',     component: Achievement,     caption: '戦績' },
      { addr: '/matchmake',       component: Matchmake,       caption: 'マッチメイク' },
      { addr: '/account',         component: Account,         caption: 'アカウント管理' },
      { addr: '/grantitem',       component: GrantItem,       caption: 'アイテム与奪' },
      { addr: '/clientLauncher',  component: ClientLauncher,  caption: 'クライアントランチャ' },
    ];
  
    return <BrowserRouter>
        <>
          <div id="contents_menu" style={{ position:"absolute", left:"0%", right:"88%", top:"0%", bottom:"0%", background:"rgba(255,255,0,0.2)" }}>
            <ul style={{listStyleType:"none"}}>
            {
              linkInfos.map((item, idx) => {
                return <li key={idx} style={{width:"100%", textAlign:"center"}}>
                  <NavLink to={item.addr}>
                    {item.caption}
                  </NavLink>
                </li>
              })
            }
            </ul>
          </div>

          <div style={{ position:"absolute", left:"12%", right:"0%", top:"0%", bottom:"0%", background:"rgba(255,128,64,0.2)" }}>
          {
            linkInfos.map((item, idx) => {
              return <Route key={idx} exact path={item.addr} component={item.component} />
            })
          }
          </div>
            
        </>
    </BrowserRouter>;

  }
}
