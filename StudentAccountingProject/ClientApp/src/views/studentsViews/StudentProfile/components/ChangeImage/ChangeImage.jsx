import React, { Component } from "react";
import CropperPage from '../cropper/CropperPage';
import {serverUrl} from '../../../../../config';
import {
    Button,
    Card,
    CardBody,
    CardGroup,
    Col,
    Form,
    Row,
  } from "reactstrap";

class ChangeImage extends Component {
  state = {
    image: '',
    croppedImage: ''  ,
    isLoading: false ,
  };

  triggerChildInput = () => {
    this.refs.cropperPage.handleClick();
  };

  getCroppedImage = img => {
    this.setState(
      {
        isLoading: true,
        croppedImage: img,
      },
    );
    this.props.changeImage(img);
  };

  changeImage = (img) => {
    this.setState({image:img});
  };

  render() {
    const { croppedImage } = this.state;

    let newImage = new Image();

    newImage.src = croppedImage;
    if (newImage.src.length < 150 || newImage.src == '') {
      newImage.src = "/UsersImages" + this.props.getUserImage();
    }

    return (
      <Card className="p-1 m-4">
        <CardBody >
          <Form onSubmit={this.onSubmitForm} >
            <Col className="xs-4">
              <CardGroup className="mb-3">
                <Card>
                  <CardBody>
                      <img style={{maxHeight:"15rem"}}
                      src={newImage.src}
                 /> 
                 </CardBody>
                </Card>
              </CardGroup>
            </Col>
            <Row>
              <Col style={{width:"100%"}}>
                <Button color="primary" className="px-5 justify-content-center" 
                onClick={this.triggerChildInput}>
                  Change image
                </Button>
              </Col>
            </Row>
          </Form>
        </CardBody>
        <CropperPage ref="cropperPage"
          getCroppedImage={this.getCroppedImage}
          changeImage={this.changeImage}
          isHidden={true}
          isForAvatar={true} />
      </Card>
    );
  }
}

  export default ChangeImage;

