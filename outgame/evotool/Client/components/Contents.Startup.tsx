import * as React from 'react';

export default class Startup extends React.Component<{}, {}> {

    render() {

      const captionStyle = {
        Position:"relative",
        color:"white",
        padding:"1em 1.2em",
        borderRadius:"5px",
        background:"linear-gradient(#666 0%, #666 50%, #333 50%, #000 100%)",
        boxShadow:"inset 1px 1px 0 rgba(0,0,0,1)",
      };

      return <div style={{ overflowY:"scroll", position:"absolute", left:"0%", right:"0%", top:"0%", bottom:"0%" }}>
        <h2 style={captionStyle}>
          evo テストプレイへの誘い
        </h2>

        <div style={{margin:"16px", background:"rgba(0,0,0,0.6)"}}>
          <h4 style={captionStyle}>
            はじめに
          </h4>
          <div style={{margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"16px", }}>
            このページは、evoを遊ぶにあたっての最低限のプレイガイドです。<br />
            evoプロジェクトの仕様は刻変化するため、必ずしも最新の内容であるとは限りません(可能な限りメンテナンスしていくつもりですが)。<br />
            また、変更点が多くなっていくため、特に変更履歴等は設けません。<br />
            皆様ついてきてください。<br />

            なにかお困りのことがあれば、evoチームメンバーまでお気軽にご相談ください！
          </div>
        </div>

        <div style={{margin:"16px", background:"rgba(0,0,0,0.6)"}}>
          <h4 style={captionStyle}>
            アカウント作成
          </h4>
          <div style={{float:"left", margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"20px", }}>
            初めてevoをプレイされる方は、まずはアカウントの作成が必要です。<br />
            左側メニューにある[アカウント管理]から[新規作成]ボタンを押してください。
          </div>
          <img style={{margin:"8px"}} src="../images/CreateAccount.gif" />
        </div>

        <div style={{margin:"16px", background:"rgba(0,0,0,0.6)"}}>
          <h4 style={captionStyle}>
            ゲームランチャーのインストール
          </h4>
          <div style={{margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"16px", }}>
            <a href="https://s3-ap-northeast-1.amazonaws.com/bno-evo-client/installer/EvoGame_setup.exe">https://s3-ap-northeast-1.amazonaws.com/bno-evo-client/installer/EvoGame_setup.exe</a><br />
            をダウンロード、実行して、ゲームランチャーをインストールしてください。<br />
	          注１)：ダウンロード時に警告が出ることがありますが、怪しいファイルではないので気にせず続行してください。<br />
	          注２)：デフォルトのインストール先は、"C:\BNO"になっています。OliveやOrangeのインストール先と被るので、気になる方は適宜変更してください。<br />
          </div>
        </div>

       <div style={{margin:"16px", background:"rgba(0,0,0,0.6)"}}>
          <h4 style={captionStyle}>
            ゲームランチャーの起動とパッチダウンロード
          </h4>
          <div style={{float:"left", margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"20px", }}>
            ランチャーを起動する前に作成されたショートカットのプロパティ→リンク先の項目に、<br />
            "半角スペース"を挟んで、以下の文字列を"追加"してください。<br />
            <br />
            <a style={{ userSelect:"text", color:"#000000", backgroundColor:"#ffffff"}}>
              /contentslist https://s3-ap-northeast-1.amazonaws.com/bno-evo-client/launcher/contents.csv
            </a><br />
            <br />
          </div>
          <img style={{margin:"8px", width:"800px"}} src="../images/contentslist_tuika.gif" />
          <div style={{margin:"8px", color:"#ffffff", lineHeight:"32px", fontSize:"16px", }}>
            その後、ランチャーを起動してください。<br />
            [ゲームスタート]ボタンを押すと、例によってパッチのダウンロードが始まります。<br />
            結構なサイズになるのと、皆でダウンロードすると時間がかかるので、お早めに済ませておいてください。<br />
          </div>
        </div>

        <div style={{margin:"16px", background:"rgba(0,0,0,0.6)"}}>
          <h4 style={captionStyle}>
            ゲーム起動後のながれ
          </h4>
          <div style={{margin:"16px", color:"#ffffff", lineHeight:"32px", fontSize:"20px", }}>
            ログインします。
            <div><img style={{marginLeft:"32px", width:"640px" }} src="../images/Login.png" /></div>
          </div>
          <div style={{margin:"16px", color:"#ffffff", lineHeight:"32px", fontSize:"20px", }}>
            [プレイ]ボタンを押して マッチメイクされるまで待機します。
            <div><img style={{marginLeft:"32px", width:"640px" }} src="../images/MatchingEntry.png" /></div>
          </div>
        </div>

      </div>;
    }
}
