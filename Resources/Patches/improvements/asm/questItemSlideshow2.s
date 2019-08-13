.thumb
push	{r4-r7,lr}
mov	r7,r9
mov	r6,r8
push	{r6-r7}
push	{r4-r7}

@check if the timer is done
ldr	r0,=#0x300100C
ldrb	r0,[r0]
mov	r1,#0x3F
and	r0,r1
cmp	r0,#0
bne	end

@check that there is at least 3 items on display
ldr	r4,=#0x2000096
ldrb	r5,[r4,#2]
cmp	r5,#0
beq	end

@check if any quest item higher than the last one is available
mov	r6,r5
b	start
loop:
mov	r0,r6
ldr	r3,=#0x807C4A8
mov	lr,r3
.short	0xF800
cmp	r0,#0
bne	match
start:
cmp	r6,#0x3C
bne	increase
mov	r6,#0x34
b	next
increase:
add	r6,#1
next:
cmp	r6,r5
bne	loop
b	end

match:
ldrb	r0,[r4]
cmp	r0,r6
beq	end
ldrb	r0,[r4,#1]
strb	r0,[r4]
ldrb	r0,[r4,#2]
strb	r0,[r4,#1]
strb	r6,[r4,#2]

ldr	r3,=#0x80A4DA8
mov	lr,r3
.short	0xF800

end:
pop	{r4-r7}
ldr	r3,=#0x80A5011
bx	r3
