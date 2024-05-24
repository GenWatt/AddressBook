class CountrySelector {
    constructor(allCountryCodes, shouldInitialize = true) {
        this.allCountryCodes = allCountryCodes;
        this.shouldInitialize = shouldInitialize;
        this.countryFlagInputId = shouldInitialize ? 'Input_CountryFlagUrl' : 'CountryFlagUrl';
        this.countryInputId = shouldInitialize ? 'Input_Country' : 'Country';
        this.initialize();
    }

    initialize() {
        $('#countrySelect').on('change', this.handleChange);

        if (this.shouldInitialize) {
            this.setUserCountry();
            this.setPhoneCode()
        } else {
            this.handleChange();
        }
    }

    handleChange = () => {
        const countryCode = $('#countrySelect').val();
        const countryData = this.allCountryCodes[countryCode];
        console.log(countryData);
        if (countryData) {
            this.updateCountryData(countryData.name, countryData.image);
        }
    }

    getCountryData() {
        const userCountryCode = navigator.language.split("-")[0];
        let userCountryData = null;
        let userCountryCodeUppercase = null;

        if (userCountryCode) {
            userCountryCodeUppercase = userCountryCode.toUpperCase();
            userCountryData = this.allCountryCodes[userCountryCodeUppercase];
        }

        return { userCountryCodeUppercase, userCountryData };
    }

    setPhoneCode() {
        const { userCountryCodeUppercase, userCountryData } = this.getCountryData();

        if (userCountryData) {
            $('#PhoneCodeSelect').val(userCountryCodeUppercase);
        }
    }

    setUserCountry() {
        const { userCountryCodeUppercase, userCountryData } = this.getCountryData();

        if (userCountryData) {
            $('#countrySelect').val(userCountryCodeUppercase);
            this.updateCountryData(userCountryData.name, userCountryData.image);
        }
    }

    updateCountryData(name, url) {
        this.updateCountryName(name);
        this.updateCountryFlag(url, name);
    }

    updateCountryFlag(url, name) {
        $('#countryFlag').attr('src', url);
        $('#countryFlag').attr('alt', name);
        $(`#${this.countryFlagInputId}`).val(url);
    }

    updateCountryName(name) {
        $(`#${this.countryInputId}`).val(name);
    }
}
