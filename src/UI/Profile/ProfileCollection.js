var Backbone = require('backbone');
var ProfileModel = require('./ProfileModel');

module.exports = (function(){
    var ProfileCollection = Backbone.Collection.extend({
        model : ProfileModel,
        url   : window.NzbDrone.ApiRoot + '/profile'
    });
    var profiles = new ProfileCollection();
    profiles.fetch();
    return profiles;
}).call(this);