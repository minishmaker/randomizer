.thumb

push	{lr}
push	{r4-r6}

	@ get the data we need
	ldr	r4, list
	ldr	r1, =#0x3000BF0
	ldrb	r5, [r1, #4] @ area
	ldrb	r6, [r1, #5] @ room
	@ for each entry in the list
	loop:
		@ check for the terminator
		ldr	r1, [r4]
		cmp	r1, #0
		beq	end
		@ check area
		ldrb	r1, [r4, #0]
		cmp	r1, r5
		bne	next
		@ check room
		ldrb	r1, [r4, #1]
		cmp	r1, r6
		beq	match
		@ go to next entry
		next:
		add	r4, #6
	b	loop

@ right area and room, spawn the item
match:
ldrb	r1, [r4, #2]
ldrb	r2, [r4, #3]
ldr	r3, =#0x80542D4
mov	lr, r3
.short	0xF800

@ give the item the flag we want
ldrh	r0, [r4, #4]
mov	r2, #0x86
strh	r0, [r1, r2]
	
end:
pop	{r4-r6}
pop	{pc}

.align
.ltorg
list:
