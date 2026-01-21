// Main JavaScript file for Pet Pals Adoption Center

// Wait for DOM to be fully loaded before running code
document.addEventListener('DOMContentLoaded', function() {
    
    // ==========================================
    // 1. EVENT HANDLING IMPLEMENTATION
    // ==========================================
    
    // ---- Button Click Events ----
    const modeToggle = document.getElementById('mode-toggle');
    modeToggle.addEventListener('click', toggleDarkMode);
    
    // ---- Hover Effects ----
    const infoItems = document.querySelectorAll('.info-item');
    infoItems.forEach(item => {
        item.addEventListener('mouseenter', handleInfoHover);
        item.addEventListener('mouseleave', handleInfoLeave);
    });
    
    // ---- Keypress Detection ----
    document.addEventListener('keydown', handleKeyPress);
    
    // ---- Secret Action (Double-Click) ----
    const headerTitle = document.querySelector('header h1');
    headerTitle.addEventListener('dblclick', revealSecretMessage);
    
    // ---- Long Press Event ----
    let longPressTimer;
    const petGallery = document.getElementById('pet-gallery');
    
    petGallery.addEventListener('mousedown', function(e) {
        longPressTimer = setTimeout(() => {
            shufflePets();
        }, 1000);
    });
    
    petGallery.addEventListener('mouseup', function() {
        clearTimeout(longPressTimer);
    });
    
    petGallery.addEventListener('mouseleave', function() {
        clearTimeout(longPressTimer);
    });
    
    // ==========================================
    // 2. INTERACTIVE ELEMENTS IMPLEMENTATION
    // ==========================================
    
    // ---- Pet Image Gallery/Slideshow ----
    const petImages = document.querySelectorAll('.pet-image');
    const prevBtn = document.getElementById('prev-btn');
    const nextBtn = document.getElementById('next-btn');
    const petName = document.getElementById('pet-name');
    let currentPetIndex = 0;
    
    // Set up gallery navigation
    prevBtn.addEventListener('click', showPreviousPet);
    nextBtn.addEventListener('click', showNextPet);
    
    // ---- Tabs Implementation ----
    const tabButtons = document.querySelectorAll('.tab-btn');
    tabButtons.forEach(button => {
        button.addEventListener('click', function() {
            const tabId = this.getAttribute('data-tab');
            switchTab(tabId);
        });
    });
    
    // ---- Help Menu Toggle ----
    const keyCommands = document.getElementById('key-commands');
    const closeHelp = document.getElementById('close-help');
    
    // Make sure the close button works
    closeHelp.addEventListener('click', function(event) {
        event.stopPropagation();
        console.log('Close help clicked');
        keyCommands.classList.add('hidden');
    });
    
    // ==========================================
    // 3. FORM VALIDATION IMPLEMENTATION
    // ==========================================
    const form = document.getElementById('interest-form');
    const nameInput = document.getElementById('full-name');
    const emailInput = document.getElementById('email');
    const phoneInput = document.getElementById('phone');
    const petSelect = document.getElementById('pet-interest');
    const passwordInput = document.getElementById('password');
    const messageInput = document.getElementById('message');
    const termsCheckbox = document.getElementById('terms');
    const formSuccess = document.getElementById('form-success');
    const resetFormBtn = document.getElementById('reset-form');
    
    // Real-time validation during typing
    nameInput.addEventListener('input', validateName);
    emailInput.addEventListener('input', validateEmail);
    phoneInput.addEventListener('input', validatePhone);
    passwordInput.addEventListener('input', validatePassword);
    messageInput.addEventListener('input', validateMessage);
    
    // Form submission
    form.addEventListener('submit', handleFormSubmit);
    
    // Reset form button
    resetFormBtn.addEventListener('click', function() {
        form.reset();
        formSuccess.classList.add('hidden');
        form.classList.remove('hidden');
        
        // Clear all error messages
        document.querySelectorAll('.error-message').forEach(error => {
            error.textContent = '';
        });
        
        // Reset password strength indicator
        const strengthBar = document.querySelector('.strength-bar');
        strengthBar.style.width = '0';
        strengthBar.style.backgroundColor = '#ddd';
        document.querySelector('.strength-text').textContent = 'Password strength';
        
        // Reset character counter
        document.getElementById('char-count').textContent = '0';
    });
    
    // Character counter for message
    messageInput.addEventListener('input', updateCharacterCount);
    
    // ==========================================
    // EVENT HANDLER FUNCTIONS
    // ==========================================
    
    // Toggle dark mode function
    function toggleDarkMode() {
        document.body.classList.toggle('dark-mode');
        const isDarkMode = document.body.classList.contains('dark-mode');
        modeToggle.textContent = isDarkMode ? 'Toggle Light Mode' : 'Toggle Dark Mode';
        
        // Animation for mode toggle
        modeToggle.classList.add('active');
        setTimeout(() => {
            modeToggle.classList.remove('active');
        }, 300);
    }
    
    // Info item hover functions
    function handleInfoHover(e) {
        // The CSS handles the tooltip display using data attributes
        e.target.style.fontWeight = 'bold';
    }
    
    function handleInfoLeave(e) {
        e.target.style.fontWeight = 'normal';
    }
    
    // Key press handler
    function handleKeyPress(e) {
        console.log('Key pressed:', e.key);
        const key = e.key.toLowerCase();
        
        // Handle arrow keys for gallery navigation
        if (key === 'arrowleft') {
            showPreviousPet();
        } else if (key === 'arrowright') {
            showNextPet();
        }
        // Number keys 1-3 for tab navigation
        else if (key === '1') {
            switchTab('care');
        } else if (key === '2') {
            switchTab('adoption');
        } else if (key === '3') {
            switchTab('stories');
        }
        // "?" key toggles help menu
        else if (key === '/' && e.shiftKey) {  // This is how "?" is typically entered
            toggleKeyCommands(!keyCommands.classList.contains('hidden'));
        } else if (key === '?') {  // Direct ? key for keyboards that support it
            toggleKeyCommands(!keyCommands.classList.contains('hidden'));
        }
        // "d" key toggles dark mode
        else if (key === 'd') {
            toggleDarkMode();
        }
        // Escape key closes any popups
        else if (key === 'escape') {
            toggleKeyCommands(false);
        }
    }
    
    // Secret message reveal on double-click
    function revealSecretMessage() {
        const secretMessage = document.getElementById('secret-message');
        secretMessage.classList.remove('hidden');
        
        // Hide the message after 3 seconds
        setTimeout(() => {
            secretMessage.classList.add('hidden');
        }, 3000);
    }
    
    // Pet gallery functions
    function showPreviousPet() {
        petImages[currentPetIndex].classList.remove('active');
        currentPetIndex = (currentPetIndex - 1 + petImages.length) % petImages.length;
        petImages[currentPetIndex].classList.add('active');
        updatePetName();
    }
    
    function showNextPet() {
        petImages[currentPetIndex].classList.remove('active');
        currentPetIndex = (currentPetIndex + 1) % petImages.length;
        petImages[currentPetIndex].classList.add('active');
        updatePetName();
    }
    
    function updatePetName() {
        const currentPet = petImages[currentPetIndex].getAttribute('data-pet');
        petName.textContent = `Meet ${currentPet}`;
        
        // Add a little animation to the pet name
        petName.style.transform = 'scale(1.1)';
        setTimeout(() => {
            petName.style.transform = 'scale(1)';
        }, 200);
    }
    
    // Long press bonus feature - shuffle pets
    function shufflePets() {
        let newIndex;
        do {
            newIndex = Math.floor(Math.random() * petImages.length);
        } while (newIndex === currentPetIndex);
        
        petImages[currentPetIndex].classList.remove('active');
        currentPetIndex = newIndex;
        petImages[currentPetIndex].classList.add('active');
        updatePetName();
        
        // Add a visual indicator that shuffle happened
        petGallery.style.backgroundColor = 'rgba(255, 126, 95, 0.1)';
        setTimeout(() => {
            petGallery.style.backgroundColor = '';
        }, 500);
    }
    
    // Tab switching function
    function switchTab(tabId) {
        // Remove active class from all tabs and panels
        document.querySelectorAll('.tab-btn').forEach(tab => {
            tab.classList.remove('active');
        });
        
        document.querySelectorAll('.tab-panel').forEach(panel => {
            panel.classList.remove('active');
        });
        
        // Add active class to selected tab and panel
        document.querySelector(`[data-tab="${tabId}"]`).classList.add('active');
        document.getElementById(tabId).classList.add('active');
    }
    
    // Helper to show/hide the key commands overlay
    function toggleKeyCommands(show) {
        if (show) {
            keyCommands.classList.remove('hidden');
        } else {
            keyCommands.classList.add('hidden');
        }
    }

    // Add a test button to help debug
    const footer = document.querySelector('footer');
    const testButton = document.createElement('button');
    testButton.textContent = 'Show Help';
    testButton.addEventListener('click', function() {
        toggleKeyCommands(true);
    });
    footer.appendChild(testButton);
    
    // Validate name field
    function validateName() {
        const nameError = document.getElementById('name-error');
        const name = nameInput.value.trim();
        
        if (name === '') {
            nameError.textContent = 'Name is required';
            nameInput.classList.add('input-error');
            return false;
        } else if (name.length < 2) {
            nameError.textContent = 'Name must be at least 2 characters';
            nameInput.classList.add('input-error');
            return false;
        } else {
            nameError.textContent = '';
            nameInput.classList.remove('input-error');
            return true;
        }
    }
    
    // Validate email field
    function validateEmail() {
        const emailError = document.getElementById('email-error');
        const email = emailInput.value.trim();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (email === '') {
            emailError.textContent = 'Email is required';
            emailInput.classList.add('input-error');
            return false;
        } else if (!emailRegex.test(email)) {
            emailError.textContent = 'Please enter a valid email address';
            emailInput.classList.add('input-error');
            return false;
        } else {
            emailError.textContent = '';
            emailInput.classList.remove('input-error');
            return true;
        }
    }
    
    // Validate phone field (optional)
    function validatePhone() {
        const phoneError = document.getElementById('phone-error');
        const phone = phoneInput.value.trim();
        const phoneRegex = /^\d{10}$|^\d{3}-\d{3}-\d{4}$|^\(\d{3}\)\s?\d{3}-\d{4}$/;
        
        if (phone !== '' && !phoneRegex.test(phone)) {
            phoneError.textContent = 'Please enter a valid 10-digit phone number';
            phoneInput.classList.add('input-error');
            return false;
        } else {
            phoneError.textContent = '';
            phoneInput.classList.remove('input-error');
            return true;
        }
    }
    
    // Validate pet selection
    function validatePetSelection() {
        const petError = document.getElementById('pet-error');
        
        if (petSelect.value === '') {
            petError.textContent = 'Please select a pet';
            petSelect.classList.add('input-error');
            return false;
        } else {
            petError.textContent = '';
            petSelect.classList.remove('input-error');
            return true;
        }
    }
    
    // Validate password and show strength meter
    function validatePassword() {
        const passwordError = document.getElementById('password-error');
        const password = passwordInput.value;
        const strengthBar = document.querySelector('.strength-bar');
        const strengthText = document.querySelector('.strength-text');
        
        // Password strength criteria
        const hasMinLength = password.length >= 8;
        const hasUpperCase = /[A-Z]/.test(password);
        const hasLowerCase = /[a-z]/.test(password);
        const hasNumbers = /\d/.test(password);
        const hasSpecialChars = /[!@#$%^&*(),.?":{}|<>]/.test(password);
        
        // Calculate password strength
        let strength = 0;
        let strengthMessage = '';
        
        if (hasMinLength) strength += 1;
        if (hasUpperCase) strength += 1;
        if (hasLowerCase) strength += 1;
        if (hasNumbers) strength += 1;
        if (hasSpecialChars) strength += 1;
        
        // Update strength bar
        strengthBar.style.width = `${strength * 20}%`;
        
        // Set color and message based on strength
        if (strength === 0) {
            strengthBar.style.backgroundColor = '#ddd';
            strengthMessage = 'Password strength';
        } else if (strength <= 2) {
            strengthBar.style.backgroundColor = '#f44336';  // Weak - Red
            strengthMessage = 'Weak password';
        } else if (strength <= 3) {
            strengthBar.style.backgroundColor = '#ff9800';  // Medium - Orange
            strengthMessage = 'Medium strength';
        } else if (strength <= 4) {
            strengthBar.style.backgroundColor = '#4caf50';  // Strong - Green
            strengthMessage = 'Strong password';
        } else {
            strengthBar.style.backgroundColor = '#2196f3';  // Very Strong - Blue
            strengthMessage = 'Very strong password';
        }
        
        strengthText.textContent = strengthMessage;
        
        // Validation rules
        if (password === '') {
            passwordError.textContent = 'Password is required';
            passwordInput.classList.add('input-error');
            return false;
        } else if (password.length < 8) {
            passwordError.textContent = 'Password must be at least 8 characters';
            passwordInput.classList.add('input-error');
            return false;
        } else if (strength <= 2) {
            passwordError.textContent = 'Password is too weak, add uppercase, numbers or special characters';
            passwordInput.classList.add('input-error');
            return false;
        } else {
            passwordError.textContent = '';
            passwordInput.classList.remove('input-error');
            return true;
        }
    }
    
    // Validate message and update character count
    function validateMessage() {
        const messageError = document.getElementById('message-error');
        const message = messageInput.value.trim();
        
        if (message === '') {
            messageError.textContent = 'Please tell us why you want to adopt this pet';
            messageInput.classList.add('input-error');
            return false;
        } else if (message.length < 10) {
            messageError.textContent = 'Your message is too short';
            messageInput.classList.add('input-error');
            return false;
        } else {
            messageError.textContent = '';
            messageInput.classList.remove('input-error');
            return true;
        }
    }
    
    // Update character count for message field
    function updateCharacterCount() {
        const charCount = document.getElementById('char-count');
        const maxLength = 200;
        const currentLength = messageInput.value.length;
        
        charCount.textContent = currentLength;
        
        if (currentLength > maxLength) {
            charCount.style.color = 'var(--error-color)';
            messageInput.value = messageInput.value.substring(0, maxLength);
            charCount.textContent = maxLength;
        } else {
            charCount.style.color = '';
        }
    }
    
    // Validate terms checkbox
    function validateTerms() {
        const termsError = document.getElementById('terms-error');
        
        if (!termsCheckbox.checked) {
            termsError.textContent = 'You must agree to the terms';
            return false;
        } else {
            termsError.textContent = '';
            return true;
        }
    }
    
    // Handle form submission
    function handleFormSubmit(e) {
        e.preventDefault();
        
        // Validate all fields
        const isNameValid = validateName();
        const isEmailValid = validateEmail();
        const isPhoneValid = validatePhone();
        const isPetValid = validatePetSelection();
        const isPasswordValid = validatePassword();
        const isMessageValid = validateMessage();
        const areTermsAccepted = validateTerms();
        
        // If all validations pass, submit the form
        if (isNameValid && isEmailValid && isPhoneValid && isPetValid && 
            isPasswordValid && isMessageValid && areTermsAccepted) {
            
            // Show success message
            form.classList.add('hidden');
            formSuccess.classList.remove('hidden');
            
            // You would typically send the form data to a server here
            console.log('Form submitted successfully with data:', {
                name: nameInput.value,
                email: emailInput.value,
                phone: phoneInput.value,
                pet: petSelect.value,
                message: messageInput.value
            });
        }
    }
    
    // Initialize the page
    function init() {
        // Show the first pet
        petImages[0].classList.add('active');
        updatePetName();
        
        // Initialize character counter
        updateCharacterCount();
    }
    
    // Run initialization
    init();
});