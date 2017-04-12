
bool dataReceived;

byte userInput[2];
byte stepsCount;


// the setup function runs once when you press reset or power the board
void setup() {
  // initialize digital pin 13 as an output.
  pinMode(7, OUTPUT);
  Serial.begin(9600);
}

// the loop function runs over and over again forever
void loop() {
  getDataFromSerial();
 
     if(dataReceived){
      for (byte i = 0; i < stepsCount; i++ ){
          digitalWrite(7, HIGH);   // turn the LED on (HIGH is the voltage level)
          delay(100);              // wait for a second
          digitalWrite(7, LOW);    // turn the LED off by making the voltage LOW
          delay(100); 
          }
   }      
}

void getDataFromSerial(){
  
    if (Serial.available() >= 2) {    
    // Read the first byte
   byte startbyte = Serial.read();
    // If it's really the startbyte (255) ...
    if (startbyte == 255) {
      // ... then get the next byte
      for (byte i=0;i<1;i++) {
        userInput[i] = Serial.read();
      }
      stepsCount = userInput[0];
   } 
   dataReceived = true;
 }else{
  dataReceived = false;
  }
}
