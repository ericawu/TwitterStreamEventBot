# TwitterStreamEventBot
Get real time information on what's exciting at the Olympics! If you're interested in a topic, tell the bot you want to follow that topic and the bot will ping you if anything exciting is happening related to that topic. You can also ask the bot what the trending topics are right now to see a list of keywords people are using on Twitter.

Step by step instructions on how to build the bot: https://docs.google.com/document/d/1vNbnFmud28feGHdurloQmXMxOiiy5r5tccOQI_D8l4w/edit?usp=sharing 

#Add on Skype:
@olympics-bot
If you can't find it, that means it's still in review. You can also download from this link: https://join.skype.com/bot/d77d39d5-ed88-4efb-8b49-1b5c3b18b0a8 
However, that link is meant to be for private demos so only the first 100 subscribers will be able to add it to their Skype. 

#Try it out!

To run this bot, you’re going to need Twitter Access tokens, an Azure account, and a cognitive services instance on Azure.

Twitter: https://dev.twitter.com/oauth/overview/application-owner-access-tokens 

Azure: https://azure.microsoft.com/en-us/ 

Text Analytics Sign Up: https://azure.microsoft.com/en-us/documentation/articles/cognitive-services-text-analytics-quick-start/ 

Add your keys to PublicConstants.cs. 

Now, it’s time to run the program! If you don’t have a bot emulator, go to https://docs.botframework.com/en-us/tools/bot-framework-emulator/ for instructions on how to download. Once you have it, you’re ready to go!

Note: if you want to add more functionality to the bot, you will need to create your own LUIS model and replace the intents in EventBotDialogue and the tokens in PublicConstants.cs. To train a model, head over to https://luis.ai 
