FROM python:3.10-bullseye

ENV PYTHON_WORKDIR = "/workdir"
ENV IN_DOCKER = "True"

RUN apt-get update
RUN python -m pip install --upgrade pip

COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY App .

EXPOSE 5500

ENTRYPOINT ["python", "app.py"]