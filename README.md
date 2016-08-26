# TwitterStreamEventBot
Get real time information on what's exciting at the Olympics! If you're interested in a topic, tell the bot you want to follow that topic and the bot will ping you if anything exciting is happening related to that topic. You can also ask the bot what the trending topics are right now to see a list of keywords people are using on Twitter.

#Add on Skype:
@olympics-bot

#Try it out!

To run this bot, you’re going to need Twitter Access tokens, an Azure account, and a cognitive services instance on Azure.

Twitter: https://dev.twitter.com/oauth/overview/application-owner-access-tokens 

Azure: https://azure.microsoft.com/en-us/ 

Text Analytics Sign Up: https://azure.microsoft.com/en-us/documentation/articles/cognitive-services-text-analytics-quick-start/ 

Add your keys to PublicConstants.cs. 

Now, it’s time to run the program! If you don’t have a bot emulator, go to https://docs.botframework.com/en-us/tools/bot-framework-emulator/ for instructions on how to download. Once you have it, you’re ready to go!

Note: if you want to add more functionality to the bot, you will need to create your own LUIS model and replace the intents in EventBotDialogue and the tokens in PublicConstants.cs. To train a model, head over to https://luis.ai 
