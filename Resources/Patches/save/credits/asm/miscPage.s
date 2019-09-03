.equ scrollingPage, miscPageData+4
.equ drawText, scrollingPage+4
.equ drawNumber, drawText+4
.equ drawTime, drawNumber+4
.equ highCredits, drawTime+4
.equ timesCredits, highCredits+4
.equ timeCredits, timesCredits+4
.thumb
push	{r4-r7,lr}
ldr	r4,=#0x20000B0
ldr	r5,miscPageData

mov	r0,r5
ldrb	r3,scrollingPage
mov	lr,r3
.short	0xF800

@check if the current line is the same as last frame
ldrb	r0,[r4,#10]
cmp	r0,#0
beq	end
mov	r0,#0
strb	r0,[r4,#10]

@clear the text background
ldr	r0,=#0x600F800
ldr	r1,=#0x600FD00
mov	r2,#0
bgcleantext:
str	r2,[r0]
add	r0,#4
cmp	r0,r1
bne	bgcleantext

@draw first entry
ldrh	r0,[r4,#6]
cmp	r0,#0
beq	skipfirst
sub	r0,#1
mov	r1,#0
bl	drawEntry
skipfirst:

@draw second entry
ldrh	r0,[r4,#6]
mov	r1,#4
bl	drawEntry

@draw third entry
ldrh	r0,[r4,#6]
add	r0,#1
ldrh	r1,[r4,#8]
cmp	r0,r1
bhs	end
mov	r1,#8
bl	drawEntry

@draw fourth entry
ldrh	r0,[r4,#6]
add	r0,#2
ldrh	r1,[r4,#8]
cmp	r0,r1
bhs	end
mov	r1,#12
bl	drawEntry

@draw fifth entry
ldrh	r0,[r4,#6]
add	r0,#3
ldrh	r1,[r4,#8]
cmp	r0,r1
bhs	end
mov	r1,#16
bl	drawEntry

end:
pop	{r4-r7,pc}

drawEntry:
push	{r4-r7,lr}
mov	r4,r0	@id
mov	r5,r1	@y
ldr	r0,miscPageData
mov	r1,#12
mul	r4,r1
add	r4,r0	@offset of entry
@draw stat name
ldr	r0,[r4]
mov	r1,#3
mov	r2,r5
ldr	r3,drawText
mov	lr,r3
mov	r3,#0
.short	0xF800
@check if timer or not
ldr	r0,[r4,#4]
cmp	r0,#0
beq	timer
@draw times used label
ldr	r0,timesCredits
mov	r1,#3
mov	r2,r5
add	r2,#1
ldr	r3,drawText
mov	lr,r3
mov	r3,#0
.short	0xF800
@draw times used, or too high
ldr	r0,[r4,#4]
ldrh	r0,[r0]
ldr	r1,=#0xFFFF
cmp	r0,r1
beq	toohigh
mov	r1,#17
mov	r2,r5
add	r2,#1
ldr	r3,drawNumber
mov	lr,r3
mov	r3,#0
.short	0xF800
b	enddraw
toohigh:
ldr	r0,highCredits
mov	r1,#17
mov	r2,r5
add	r2,#1
ldr	r3,drawText
mov	lr,r3
mov	r3,#0
.short	0xF800
b	enddraw
timer:
@draw timer label
ldr	r0,timeCredits
mov	r1,#3
mov	r2,r5
add	r2,#1
ldr	r3,drawText
mov	lr,r3
mov	r3,#0
.short	0xF800
@draw timer
ldr	r0,[r4,#8]
ldr	r0,[r0]
mov	r1,#17
mov	r2,r5
add	r2,#1
ldr	r3,drawTime
mov	lr,r3
mov	r3,#0
.short	0xF800
enddraw:
pop	{r4-r7,pc}

.align
.ltorg
miscPageData:
@POIN miscPageData
@POIN scrollingPage
@POIN drawText
@POIN drawNumber
@POIN drawTime
@POIN highCredits
@POIN timesCredits
@POIN timeCredits
