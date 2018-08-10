## Dependencies

```bash
### Install Python + Dependencies
sudo apt-get install -y python-dev
sudo apt-get install -y python-pip
sudo apt-get install -y python-numpy python-dev cmake zlib1g-dev libjpeg-dev xvfb xorg-dev python-opengl libboost-all-dev libsdl2-dev swig

sudo pip install werkzeug
sudo pip install itsdangerous
sudo pip install click

# Export our display settings for XMing
export DISPLAY=localhost:0.0
echo 'export DISPLAY=localhost:0.0 ' >> ~/.bashrc
```

## Install

```bash
sudo pip install -r requirements.txt
```

## Run

```bash
python gym_http_server.py
```