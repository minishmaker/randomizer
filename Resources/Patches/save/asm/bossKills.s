.thumb
push	{r4-r7,lr}
mov	r4,r0
mov	r5,r1
mov	r2,r0
cmp	r1,#0
beq	end
ldr	r0,=#0x2002C9C
add	r1,r2
ldr	r3,=#0x801D5F4
mov	lr,r3
.short	0xF800

@check if chu
cmp	r4,#0
bne	notChu
cmp	r5,#2
bne	notChu
@set chu time
mov	r0,#0
bl	setTime
b	end
notChu:

@check if gleerok
cmp	r4,#0
bne	notGleerok
cmp	r5,#3
bne	notGleerok
@set gleerok time
mov	r0,#1
bl	setTime
b	end
notGleerok:

@check if mazaal
ldr	r2,=#0x680
cmp	r4,r2
bne	notMazaal
cmp	r5,#0x31
bne	notMazaal
@set mazaal time
mov	r0,#2
bl	setTime
b	end
notMazaal:

@check if octo
cmp	r4,#0
bne	notOcto
cmp	r5,#5
bne	notOcto
@set octo time
mov	r0,#3
bl	setTime
b	end
notOcto:

@check if gyorg
ldr	r2,=#0x800
cmp	r4,r2
bne	notGyorg
cmp	r5,#0x7B
bne	notGyorg
@set gyorg time
mov	r0,#4
bl	setTime
b	end
notGyorg:

@check if blue chu
ldr	r2,=#0x740
cmp	r4,r2
bne	notBlue
cmp	r5,#0x48
bne	notBlue
@set vaati2 time
mov	r0,#5
bl	setTime
b	end
notBlue:

@chack if vaati3
cmp	r4,#0
bne	notVaati3
cmp	r5,#0x51
bne	notVaati3
@set vaati3 time
mov	r0,#7
bl	setTime
b	end
notVaati3:

end:
pop	{r4-r7,pc}

setTime:
lsl	r0,#2
ldr	r1,offset
ldr	r2,[r1,r0]
cmp	r2,#0
bne	end
ldr	r2,=#0x203FFF0
ldr	r2,[r2]
str	r2,[r1,r0]
b	end

.align
.ltorg
offset:
