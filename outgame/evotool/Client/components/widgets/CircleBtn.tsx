import * as React from 'react';



interface Property {
  enable: boolean;
  text:string;
  size:number;
  color:number;
  fontSize:number;
  onClick: () => void;
  href: string;
}
type Props = Property;


export default class CircleBtn extends React.Component<Props, {}>{

  render() {

    const r1 = ((this.props.color>>16)&0xff).toString(10);
    const g1 = ((this.props.color>> 8)&0xff).toString(10);
    const b1 = ((this.props.color>> 0)&0xff).toString(10);
    const r2 = (((this.props.color>>16)&0xff)*0.7).toString(10);
    const g2 = (((this.props.color>> 8)&0xff)*0.7).toString(10);
    const b2 = (((this.props.color>> 0)&0xff)*0.7).toString(10);
    
    const style = {
      margin: (this.props.size * 0.1) + "px",
      background: `rgba(${r1},${g1},${b1},1.0)`,
      borderBottom: `solid 4px rgba(${r2},${g2},${b2},1.0)`,
      color: "#FFF",
      width: this.props.size + "px",
      height: this.props.size + "px",
      lineHeight: this.props.size + "px",
      fontSize: this.props.fontSize + "px",
    };

    if (this.props.enable) {
      return <a href={this.props.href} className="circlebtn" style={style} onClick={(e) => { this.props.onClick(); }} >
        {this.props.text}
      </a>;
    }

    return <></>;

  }
}
